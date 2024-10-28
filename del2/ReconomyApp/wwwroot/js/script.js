document.addEventListener('DOMContentLoaded', function() {
    const earlyCheck = document.getElementById('early-check');
    const reasonContainer = document.getElementById('reason-container');
    const earlyDepartureContainer = document.getElementById('early-departure-container');
    const statusRadios = document.getElementsByName('status');
    const ovrigtRadio = document.getElementById('ovrigt-radio');
    const ovrigtContainer = document.getElementById('Övrigt-container');
    const checkInForm = document.getElementById('check-in-form');
    const successPopup = document.getElementById('success-popup');
    const closePopupButton = document.getElementById('close-popup');

    // Show or hide the reason field based on the early check checkbox
    earlyCheck.addEventListener('change', function() {
        reasonContainer.style.display = this.checked ? 'block' : 'none';
    });

    // Show or hide the övrigt container based on the selected status
    statusRadios.forEach(radio => {
        radio.addEventListener('change', function() {
            if (this.value === 'gick') {
                earlyDepartureContainer.style.display = 'block';
                ovrigtContainer.style.display = 'none'; // Hide "Övrigt" container
                earlyCheck.checked = false;
                reasonContainer.style.display = 'none';
            } else if (this.value === 'kom') {
                earlyDepartureContainer.style.display = 'none';
                ovrigtContainer.style.display = 'none'; // Hide "Övrigt" container
                earlyCheck.checked = false;
                reasonContainer.style.display = 'none';
            } else if (this.value === 'ovrigt') {
                earlyDepartureContainer.style.display = 'none'; // Hide early departure container
                ovrigtContainer.style.display = 'block'; // Show "Övrigt" container
            }
        });
    });

    // Handle the form submission
    checkInForm.addEventListener('submit', function(event) {
        event.preventDefault(); // Prevent the default form submission

        // Show the success pop-up
        successPopup.style.display = 'flex';

        // Reset the form after the pop-up is closed
        closePopupButton.addEventListener('click', function() {
            successPopup.style.display = 'none'; // Hide the pop-up
            checkInForm.reset(); // Reset the form fields
            ovrigtContainer.style.display = 'none'; // Hide "Övrigt" container
            earlyDepartureContainer.style.display = 'none'; // Hide "Early Departure" container
        });
    });

    // Populate the participant dropdown
    const participants = ['Anna Svensson', 'Björn Johansson', 'Cecilia Karlsson'];
    const participantSelect = document.getElementById('participant');

    participants.forEach(name => {
        const option = document.createElement('option');
        option.value = name;
        option.textContent = name;
        participantSelect.appendChild(option);
    });
});
// page 2 
document.addEventListener('DOMContentLoaded', function () {
    const namnInput = document.getElementById('namnInput');
    const addNamnButton = document.getElementById('addNamnButton');
    const participantSelect = document.getElementById('participant');
    const removeParticipantSelect = document.getElementById('removeParticipant');
    const removeNamnButton = document.getElementById('removeNamnButton');

    addNamnButton.addEventListener('click', function () {
        const namn = namnInput.value.trim();
        if (namn) {
            const option = document.createElement('option');
            option.value = namn;
            option.text = namn;
            participantSelect.appendChild(option);

            // Lägg till i ta bort-listan
            const removeOption = option.cloneNode(true);
            removeParticipantSelect.appendChild(removeOption);

            namnInput.value = '';
        }
    });

    removeNamnButton.addEventListener('click', function () {
        const selectedNamn = removeParticipantSelect.value;
        if (selectedNamn) {
            // Ta bort från deltagarlistan
            Array.from(participantSelect.options).forEach(option => {
                if (option.value === selectedNamn) {
                    option.remove();
                }
            });

            // Ta bort från ta bort-listan
            Array.from(removeParticipantSelect.options).forEach(option => {
                if (option.value === selectedNamn) {
                    option.remove();
                }
            });
        }
    });
});
