// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MasonJar.Common;

namespace MasonJar
{
    [Activity(Label = "EditActivity")]
    public class EditActivity : ListActivity
    {
        private ViewModel.Jar      _JarViewModel;
        private EditAdapter        _Adapter;
        private RelativeLayout     _OverlayCategoryDelete;
        private RelativeLayout     _OverlayCategoryRename;
        private RelativeLayout     _OverlayCategorySwatch;
        private RelativeLayout     _OverlayItemDelete;
        private RelativeLayout     _OverlayItemRename;
        private EditText           _CategoryRenameField;
        private EditText           _ItemRenameField;
        private Button             _CategoryRenameOk;
        private Button             _ItemRenameOk;
        private ViewModel.Category _CategoryTarget;
        private ViewModel.Item     _ItemTarget;

        public delegate void CallbackDelegate();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // Setup the layout.
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Edit);
            ViewHelper.FixBackgroundRepeat(FindViewById<LinearLayout>(Resource.Id.maineditlayout));

            // Get the view model.
            _JarViewModel = ViewModel.Jar.GetInstance();
            _JarViewModel.CategoryCollectionChanged += CategoryOrItemListChanged;
            _JarViewModel.ItemCollectionChanged += CategoryOrItemListChanged;

            // Create the headers and make the buttons hook up to this activity.
            LayoutInflater inflater = (LayoutInflater)GetSystemService(Context.LayoutInflaterService);

            View categoriesHeaderView = inflater.Inflate(Resource.Layout.EditHeaderCategories, null);
            categoriesHeaderView.FindViewById<ImageButton>(Resource.Id.edit_button_add_category).Click += AddCategoryClicked;

            View itemsHeaderView = inflater.Inflate(Resource.Layout.EditHeaderItems, null);
            itemsHeaderView.FindViewById<ImageButton>(Resource.Id.edit_button_add_item).Click += AddItemClicked;

            // Create delegates for items and categories.
            EditAdapter.CallbackSet callbacks = new EditAdapter.CallbackSet();
            callbacks.CallbackCategorySwatchClicked = delegate (ViewModel.Category category) { CategorySwatchClicked(category); };
            callbacks.CallbackCategoryDeleteClicked = delegate (ViewModel.Category category) { CategoryDeleteClicked(category); };
            callbacks.CallbackCategoryTitleClicked  = delegate (ViewModel.Category category) { CategoryTitleClicked(category); };
            callbacks.CallbackItemCategoryClicked   = delegate (ViewModel.Item item)         { ItemCategoryClicked(item); };
            callbacks.CallbackItemContentClicked    = delegate (ViewModel.Item item)         { ItemContentClicked(item); };
            callbacks.CallbackItemDeleteClicked     = delegate (ViewModel.Item item)         { ItemDeleteClicked(item); };

            // Set the list view content.
            _Adapter = new EditAdapter(_JarViewModel, categoriesHeaderView, itemsHeaderView, callbacks, inflater);
            ListAdapter = _Adapter;
            _Adapter.NotifyDataSetChanged();
            ListView.RequestFocus();

            // Setup the overlays.
            _OverlayCategoryDelete = FindViewById<RelativeLayout>(Resource.Id.edit_overlay_category_delete);
            FindViewById<Button>(Resource.Id.edit_overlay_category_delete_yes).Click += CategoryDeleteYesClicked;
            FindViewById<Button>(Resource.Id.edit_overlay_category_delete_no).Click += DismissOverlays;

            _OverlayItemDelete = FindViewById<RelativeLayout>(Resource.Id.edit_overlay_item_delete);
            FindViewById<Button>(Resource.Id.edit_overlay_item_delete_yes).Click += ItemDeleteYesClicked;
            FindViewById<Button>(Resource.Id.edit_overlay_item_delete_no).Click += DismissOverlays;

            _OverlayCategoryRename = FindViewById<RelativeLayout>(Resource.Id.edit_overlay_category_rename);
            _CategoryRenameField = FindViewById<EditText>(Resource.Id.edit_overlay_category_rename_edit);
            _CategoryRenameOk = FindViewById<Button>(Resource.Id.edit_overlay_category_rename_yes);
            _CategoryRenameField.TextChanged += CategoryRenameTextChanged;
            _CategoryRenameOk.Click += CategoryRenameOkClicked;
            FindViewById<Button>(Resource.Id.edit_overlay_category_rename_no).Click += DismissOverlays;

            _OverlayItemRename = FindViewById<RelativeLayout>(Resource.Id.edit_overlay_item_rename);
            _ItemRenameField = FindViewById<EditText>(Resource.Id.edit_overlay_item_rename_edit);
            _ItemRenameOk = FindViewById<Button>(Resource.Id.edit_overlay_item_rename_yes);
            _ItemRenameField.TextChanged += ItemRenameTextChanged;
            _ItemRenameOk.Click += ItemRenameOkClicked;
            FindViewById<Button>(Resource.Id.edit_overlay_item_rename_no).Click += DismissOverlays;

            _OverlayCategorySwatch = FindViewById<RelativeLayout>(Resource.Id.edit_overlay_category_swatch);
        }

        public override void Finish()
        {
            base.Finish();
            OverridePendingTransition(0, 0);
        }

        private void AddCategoryClicked(object sender, EventArgs args)        { _JarViewModel.AddNewCategory(); }
        private void AddItemClicked(object sender, EventArgs args)            { _JarViewModel.AddNewItem(); }
        private void CategoryOrItemListChanged(object sender, EventArgs args) { _Adapter.NotifyDataSetChanged(); }
        private void CategoryRenameTextChanged(object sender, EventArgs args) { _CategoryRenameOk.Enabled = HasText(_CategoryRenameField); }
        private void ItemRenameTextChanged(object sender, EventArgs args)     { _ItemRenameOk.Enabled = HasText(_ItemRenameField); }
        private void CategoryDeleteClicked(ViewModel.Category category)       { ShowOverlay(_OverlayCategoryDelete, category); }
        private void ItemDeleteClicked(ViewModel.Item item)                   { ShowOverlay(_OverlayItemDelete, item); }
        private void CategorySwatchClicked(ViewModel.Category category)       { ShowOverlay(_OverlayCategorySwatch, category); }
        private bool HasText(EditText edit)                                   { return (edit.Text != null) && (edit.Text.Length > 0); }

        private void ItemRenameOkClicked(object sender, EventArgs args)
        {
            _ItemTarget.Content = _ItemRenameField.Text;
            DismissOverlays();
        }

        private void CategoryRenameOkClicked(object sender, EventArgs args)
        {
            _CategoryTarget.Title = _CategoryRenameField.Text;
            DismissOverlays();
        }

        private void CategoryDeleteYesClicked(object sender, EventArgs args)
        {
            _JarViewModel.DeleteCategory(_CategoryTarget);
            DismissOverlays();
        }

        private void ItemDeleteYesClicked(object sender, EventArgs args)
        {
            _JarViewModel.DeleteItem(_ItemTarget);
            DismissOverlays();
        }

        private void DismissOverlays(object sender = null, EventArgs args = null)
        {
            _CategoryTarget = null;
            _ItemTarget = null;
            _OverlayCategoryDelete.Visibility = ViewStates.Invisible;
            _OverlayItemDelete.Visibility = ViewStates.Invisible;
            _OverlayCategoryRename.Visibility = ViewStates.Invisible;
            _OverlayItemRename.Visibility = ViewStates.Invisible;
        }

        private void CategoryTitleClicked(ViewModel.Category category)
        {
            ShowOverlay(_OverlayCategoryRename, category);
            _CategoryRenameOk.Enabled = true;
            _CategoryRenameField.Text = category.Title;
        }

        private void ItemCategoryClicked(ViewModel.Item item)
        {
        }

        private void ItemContentClicked(ViewModel.Item item)
        {
            ShowOverlay(_OverlayItemRename, item);
            _ItemRenameOk.Enabled = true;
            _ItemRenameField.Text = item.Content;
        }

        private void ShowOverlay(RelativeLayout overlay, ViewModel.Category category)
        {
            _CategoryTarget = category;
            overlay.Visibility = ViewStates.Visible;
        }

        private void ShowOverlay(RelativeLayout overlay, ViewModel.Item item)
        {
            _ItemTarget = item;
            overlay.Visibility = ViewStates.Visible;
        }
    }
}