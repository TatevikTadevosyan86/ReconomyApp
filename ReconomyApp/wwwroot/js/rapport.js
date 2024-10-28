function generateReport() {
    const reportType = document.getElementById('reportTypeSelect').value;
   // const startDate = document.getElementById('reportStartDateInput').value;
   // const endDate = document.getElementById('reportEndDateInput').value;

    const reportRequest = {
        ReportType: reportType,
       // StartDate: startDate,
       // EndDate: endDate
    };

    // Update the fetch URL to call the consolidated endpoint
    fetch(`https://localhost:7062/api/reports/generate`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ reportRequest }) 
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Error generating report: ' + response.statusText);
            }
            return response.blob(); // Get the response as a Blob
        })
        .then(blob => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `${reportType}Report.xlsx`; // Use reportType to name the file dynamically
            document.body.appendChild(a);
            a.click();
            a.remove(); // Remove the link from the DOM
            window.URL.revokeObjectURL(url); // Clean up the URL object
        })
        .catch(error => console.error('Error:', error));
}
