// src/ShadowWrapper.jsx
import React, { useEffect, useRef, useState } from 'react';
import { createPortal } from 'react-dom';

export default function ShadowWrapper({ children, styles }) {
	const hostRef = useRef(null);
	const [shadowRoot, setShadowRoot] = useState(null);

	useEffect(() => {
		if (hostRef.current && !shadowRoot) {
			const sr = hostRef.current.attachShadow({ mode: 'open' });
			const linkElem = document.createElement('link');
			linkElem.rel = 'stylesheet';
			linkElem.href = `${BASE_URL}/react/build/style.css`;
			sr.appendChild(linkElem);
			setShadowRoot(sr);
		}
	}, [hostRef, shadowRoot]);

	// shadowRoot ができたら、子要素をその中に portal 経由でレンダリングする
	return (
		<div ref={hostRef}>
			{shadowRoot && createPortal(children, shadowRoot)}
		</div>
	);
}
