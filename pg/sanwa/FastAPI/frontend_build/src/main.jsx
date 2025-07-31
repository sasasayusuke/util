import React from 'react';
import ReactDOM from 'react-dom/client';
// ↓ ページを増やしたら　コンポーネントの importを追加する
import FormShiire from './pages/FormShiire.jsx';

import ShadowWrapper from './ShadowWrapper';
import './styles/index.css';
import FormUriage from './pages/FormUriage.jsx';

// ↓ ページを増やしたら　initを追加する
function initFormShiire(rootId, context) {
  const rootElement = document.getElementById(rootId);
  if (!rootElement) return;

  const root = ReactDOM.createRoot(rootElement);
  root.render(
    <ShadowWrapper /* styles={formStyles} */>
      <FormShiire context={context} />
    </ShadowWrapper>
  );
}


function initFormUriage(rootId, context) {
  const rootElement = document.getElementById(rootId);
  if (!rootElement) return;

  const root = ReactDOM.createRoot(rootElement);
  root.render(
    <ShadowWrapper /* styles={formStyles} */>
      <FormUriage context={context} />
    </ShadowWrapper>
  );
}
window.FormLib = {
// ↓ ページを増やしたら　グローバルスコープに公開を追加する
  initFormShiire,
  initFormUriage
};
