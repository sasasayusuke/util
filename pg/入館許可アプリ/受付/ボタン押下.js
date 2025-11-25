
// URLを動的に設定
document.addEventListener('DOMContentLoaded', function() {
const qrReaderLink = document.getElementById('qr-reader-link');
const receptionLink = document.getElementById('reception-link');

if (qrReaderLink && SYSTEM_CONFIG) {
	qrReaderLink.href = SYSTEM_CONFIG.SITE_INFO.QR_READER.URL;
}

if (receptionLink && SYSTEM_CONFIG) {
	receptionLink.href = SYSTEM_CONFIG.SITE_INFO.UNSCHEDULED_VISIT.URL;
}
});