import React from 'react';
import ReactDOM from 'react-dom/client';
// ↓ ページを増やしたら　コンポーネントの importを追加する
import FormShiire from './pages/FormShiire.jsx';

import ShadowWrapper from './ShadowWrapper';
import './styles/index.css';
import FormUriage from './pages/FormUriage.jsx';

// ↓ ページを増やしたら　initを追加する
const roots = new Map();

function initFormShiire(rootId, context) {
  const rootElement = document.getElementById(rootId);
  if (!rootElement) return;

  let root = roots.get(rootId);
  if (!root) {
    root = ReactDOM.createRoot(rootElement);
    roots.set(rootId, root);
  }
  
  root.render(
    <ShadowWrapper /* styles={formStyles} */>
      <FormShiire context={context} />
    </ShadowWrapper>
  );
}


function initFormUriage(rootId, context) {
  const rootElement = document.getElementById(rootId);
  if (!rootElement) return;

  let root = roots.get(rootId);
  if (!root) {
    root = ReactDOM.createRoot(rootElement);
    roots.set(rootId, root);
  }
  
  root.render(
    <ShadowWrapper /* styles={formStyles} */>
      <FormUriage context={context} />
    </ShadowWrapper>
  );
}
function cleanupRoot(rootId) {
  const root = roots.get(rootId);
  if (root) {
    // Reactルートをアンマウント
    root.unmount();
    roots.delete(rootId);
    
    // モーダル内のイベントリスナーもクリーンアップ
    const modalContainer = document.querySelector('.modal-content');
    if (modalContainer) {
      // すべてのイベントリスナーを削除
      const newModalContainer = modalContainer.cloneNode(true);
      modalContainer.parentNode.replaceChild(newModalContainer, modalContainer);
    }
  }
}

window.FormLib = {
// ↓ ページを増やしたら　グローバルスコープに公開を追加する
  initFormShiire,
  initFormUriage,
  cleanupRoot
};
