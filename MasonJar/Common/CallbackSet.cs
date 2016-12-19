// [Ready Design Corps] - [Mason Jar] - Copyright 2016

namespace MasonJar.Common
{
    public delegate void CallbackDelegateCategory(ViewModel.Category item);
    public delegate void CallbackDelegateItem(ViewModel.Item item);

    public class CallbackSet
    {
        public CallbackDelegateCategory CallbackCategorySwatchClicked;
        public CallbackDelegateCategory CallbackCategoryDeleteClicked;
        public CallbackDelegateCategory CallbackCategoryTitleClicked;
        public CallbackDelegateCategory CallbackItemCategorySelectionClicked;
        public CallbackDelegateItem     CallbackItemCategoryClicked;
        public CallbackDelegateItem     CallbackItemContentClicked;
        public CallbackDelegateItem     CallbackItemDeleteClicked;
    }
}