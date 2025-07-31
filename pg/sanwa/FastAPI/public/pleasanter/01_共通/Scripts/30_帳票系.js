
function generatePDF() {
    const element = document.getElementById('content-to-pdf');
    const opt = {
        margin: 1,
        filename: 'generated-pdf.pdf',
        image: { type: 'jpeg', quality: 0.98 },
        html2canvas: { scale: 2 },
        jsPDF: { unit: 'in', format: 'letter', orientation: 'portrait' }
    };

    html2pdf().set(opt).from(element).outputPdf('bloburl').then((blobUrl) => {
        window.open(blobUrl, '_blank');
    });
}