$(function () {
  const btn = document.querySelector(
    'button.confirm-unload[data-id="2841751"]'
  );
  if (!btn) return;

  btn.dispatchEvent(new MouseEvent('mousedown', { bubbles: true }));
  btn.dispatchEvent(new MouseEvent('mouseup', { bubbles: true }));
  btn.dispatchEvent(new MouseEvent('click', { bubbles: true }));
});