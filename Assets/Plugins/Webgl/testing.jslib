mergeInto(LibraryManager.library, {
  ReactMessage: function (message) {
    window.dispatchReactUnityEvent("ReactMessage", message);
  },
});