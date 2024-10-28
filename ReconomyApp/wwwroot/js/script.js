document.addEventListener('DOMContentLoaded', function () {
    const earlyCheck = document.getElementById('early-check');
    const reasonContainer = document.getElementById('reason-container');
    const earlyDepartureContainer = document.getElementById('early-departure-container');
    const statusRadios = document.getElementsByName('status');
    const ovrigtRadio = document.getElementById('ovrigt-radio');
    const ovrigtContainer = document.getElementById('Övrigt-container');
    const checkInForm = document.getElementById('check-in-form');
    const successPopup = document.getElementById('success-popup');
    const closePopupButton = document.getElementById('close-popup');
    const welcomeBackground = document.getElementById("welcome-background");
    const checkInFormContainer = document.getElementById("check-in-form-container");
    const successMessage = document.getElementById("success-message");
    const participantSelect = document.getElementById('participant');

    // Show or hide the reason field based on the early check checkbox
    earlyCheck.addEventListener('change', function () {
        reasonContainer.style.display = this.checked ? 'block' : 'none';
    });

    // Show or hide the övrigt container based on the selected status
    statusRadios.forEach(radio => {
        radio.addEventListener('change', function () {
            if (this.value === 'gick') {
                earlyDepartureContainer.style.display = 'block';
                ovrigtContainer.style.display = 'none';
                earlyCheck.checked = false;
                reasonContainer.style.display = 'none';
            } else if (this.value === 'kom') {
                earlyDepartureContainer.style.display = 'none';
                ovrigtContainer.style.display = 'none';
                earlyCheck.checked = false;
                reasonContainer.style.display = 'none';
            } else if (this.value === 'ovrigt') {
                earlyDepartureContainer.style.display = 'none';
                ovrigtContainer.style.display = 'block';
            }
        });
    });

    // Handle the form submission
    checkInForm.addEventListener('submit', function (event) {
        event.preventDefault(); // Prevent the default form submission

        const formData = new FormData(checkInForm);
        const participantId = formData.get('participant_name');
        const earlyDeparture = formData.get('early_departure') ? true : false;
        const reason = formData.get('reason') || "";
        const status = formData.get('status'); // Get the selected status (Check-In or Check-Out)

        const data = {
            ParticipantId: participantId,
            EarlyDeparture: earlyDeparture,
            ReasonForEarlyDeparture: reason
        };
        console.log('Request Body:', data);

        let apiEndpoint = '';
        let successHeaderText = '';
        let successMessageText = '';

        if (status === 'kom') {
            // Check-In
            apiEndpoint = '/api/checkinout/checkin';
            successHeaderText = 'Välkommen!';
            successMessageText = 'Du har checkat in.';
        } else if (status === 'gick') {
            // Check-Out
            apiEndpoint = '/api/checkinout/checkout';
            successHeaderText = 'Tack!';
            successMessageText = 'Du har checkat ut.';
        }

        // Make a fetch request to submit the data to your backend
        fetch(apiEndpoint, { // Dynamically use the correct endpoint
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                document.getElementById('success-header').textContent = successHeaderText;
                document.getElementById('success-message').textContent = successMessageText;
                // Show the success pop-up
                successPopup.style.display = 'flex';
                // Hide the form and show the welcome background after a delay (3 seconds)
                setTimeout(function () {
                    successPopup.style.display = 'none';
                    welcomeBackground.style.display = "block";
                    checkInFormContainer.style.display = 'none';
                    successMessage.style.display = "none";
                }, 3000);
            })
            .catch(error => console.error('Error submitting form:', error));
    });

    // Handle manually closing the pop-up
    closePopupButton.addEventListener('click', function () {
        successPopup.style.display = 'none';
        checkInForm.reset();
        ovrigtContainer.style.display = 'none';
        earlyDepartureContainer.style.display = 'none';
        welcomeBackground.style.display = "block";
        checkInFormContainer.style.display = 'none';
    });

    // Fetch and populate the participant dropdown
    function updateParticipantDropdown() {
        fetch('/api/participants')
            .then(response => response.json())
            .then(participants => {
                const uniqueParticipants = [];
                const seen = new Set();

                participants.forEach(participant => {
                    if (!seen.has(participant.id)) {
                        seen.add(participant.id);
                        uniqueParticipants.push(participant);
                    }
                });

                participantSelect.innerHTML = ''; // Clear existing options

                uniqueParticipants.forEach(participant => {
                    const option = document.createElement('option');
                    option.value = participant.id;
                    option.textContent = participant.name;
                    participantSelect.appendChild(option);
                });
            })
            .catch(error => console.error('Error fetching participants:', error));
    }

    updateParticipantDropdown();

    // Show the form when the "Check In/Out" button is clicked
    document.getElementById("check-in-button").addEventListener("click", function () {
        welcomeBackground.style.display = "none";
        checkInFormContainer.style.display = "block";
    });
});
