// Function to open a modal
function openModal(modalId) {
    console.log(`Opening modal: ${modalId}`); // Debug log
    document.getElementById(modalId).style.display = 'block';
}

// Function to close a modal
function closeModal(modalId) {
    document.getElementById(modalId).style.display = 'none';
}


document.addEventListener('DOMContentLoaded', function () {
    const addNamnInput = document.getElementById('addNamnInput');
    const addStartDateInput = document.getElementById('addStartDateInput');
    const addEndDateInput = document.getElementById('addEndDateInput');
    const workCommitmentInput = document.getElementById('workCommitmentInput');
    const addParticipantButton = document.getElementById('addParticipantButton');
    const updateNamnInput = document.getElementById('updateNamnInput');
    const updateStartDateInput = document.getElementById('updateStartDateInput');
    const updateEndDateInput = document.getElementById('updateEndDateInput');
    const updateWorkCommitmentInput = document.getElementById('updateWorkCommitmentInput');
    const updateParticipantButton = document.getElementById('updateParticipantButton');
    const participantTable = document.getElementById('participantTable').getElementsByTagName('tbody')[0];
    const participantListToRemove = document.getElementById('participantListToRemove');
    const removeParticipantButton = document.getElementById('removeParticipantButton');

    let currentParticipantId = null;

    // Function to refresh the table
    function loadParticipants() {
        console.log('Loading participants...');
        participantTable.innerHTML = '';  // Clear the table
        participantListToRemove.innerHTML = '';  // Clear the remove list

        fetch('/api/participants')
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                console.log('Participants fetched:', data);
                if (Array.isArray(data)) {
                    data.forEach(participant => {
                        addRowToTable(participant);
                        addParticipantToRemoveList(participant);
                    });
                } else {
                    console.error('Expected an array but got:', data);
                }
            })
            .catch(error => console.error('Error fetching participants:', error));
    }

    // Function to add a row to the table
    function addRowToTable(participant) {
        const row = document.createElement('tr');
        const nameCell = document.createElement('td');
        const startDateCell = document.createElement('td');
        const endDateCell = document.createElement('td');
        const updateCell = document.createElement('td');
        const updateButton = document.createElement('button');

        nameCell.textContent = participant.name;
        startDateCell.textContent = participant.startDate;
        endDateCell.textContent = participant.endDate;
        updateButton.textContent = 'Update';
        updateButton.classList.add('updateButton');

        // Update functionality
        updateButton.addEventListener('click', function () {
            console.log('Update button clicked'); // Debug log
            openModal('updateParticipantModal');
            updateNamnInput.value = participant.name;
            updateStartDateInput.value = participant.startDate ? participant.startDate.split('T')[0] : ''; // Change this line
            updateEndDateInput.value = participant.endDate ? participant.endDate.split('T')[0] : ''; // Change this line
            updateWorkCommitmentInput.value = participant.workCommitment;

            currentParticipantId = participant.id;
        });


        row.appendChild(nameCell);
        row.appendChild(startDateCell);
        row.appendChild(endDateCell);
        updateCell.appendChild(updateButton);
        row.appendChild(updateCell);

        participantTable.appendChild(row);
    }

    // Function to add a participant to the remove list
    function addParticipantToRemoveList(participant) {
        const listItem = document.createElement('li');
        const removeOption = document.createElement('input');
        const removeLabel = document.createElement('label');

        removeOption.type = 'radio';
        removeOption.name = 'participant';
        removeOption.value = participant.id;
        removeLabel.textContent = participant.name;

        listItem.appendChild(removeOption);
        listItem.appendChild(removeLabel);
        participantListToRemove.appendChild(listItem);
    }

    // Add participant when the button is clicked
    addParticipantButton.addEventListener('click', function () {
        const namn = addNamnInput.value.trim();
        const startDate = addStartDateInput.value;
        const endDate = addEndDateInput.value;
        const workCommitment = workCommitmentInput.value;

        if (namn && startDate && endDate) {
            fetch('/api/participants', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ name: namn, startDate, endDate, workCommitment })
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    loadParticipants();  // Refresh the table after add
                    closeModal('addParticipantModal');
                })
                .catch(error => console.error('Error adding participant:', error));
        } else {
            alert("Please fill in all required fields.");
        }
    });

    // Update participant when the button is clicked
    updateParticipantButton.addEventListener('click', function () {
        const namn = updateNamnInput.value.trim();
        const startDate = updateStartDateInput.value;
        const endDate = updateEndDateInput.value;
        const workCommitment = updateWorkCommitmentInput.value;

        if (namn && startDate && endDate && currentParticipantId && workCommitment) {
            console.log('Updating participant with ID:', currentParticipantId);
            console.log('Data being sent:', { name: namn, startDate, endDate, workCommitment });

            fetch(`/api/participants/${currentParticipantId}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ ID: currentParticipantId, name: namn, startDate, endDate, workCommitment })
            })
                .then(response => {
                    if (response.status === 204) {
                        loadParticipants();  // Refresh the table after update
                        closeModal('updateParticipantModal');
                    } else {
                        throw new Error('Network response was not ok');
                    }
                })
                .catch(error => console.error('Error updating participant:', error));
        } else {
            alert("Please fill in all required fields.");
        }
    });

    // Remove selected participant
    removeParticipantButton.addEventListener('click', function () {
        const selectedParticipant = document.querySelector('#participantListToRemove input:checked');
        if (selectedParticipant) {
            const participantId = selectedParticipant.value;
            fetch(`/api/participants/${participantId}`, { method: 'DELETE' })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    loadParticipants();  // Refresh the table after delete
                })
                .catch(error => console.error('Error deleting participant:', error));
        } else {
            alert("Please select a participant to remove.");
        }
    });



    // Event listeners for modals
    document.querySelectorAll('.close').forEach(function (element) {
        element.addEventListener('click', function () {
            closeModal(this.closest('.modal').id);
        });
    });

    // Click outside modal to close
    window.addEventListener('click', function (event) {
        if (event.target.classList.contains('modal')) {
            closeModal(event.target.id);
        }
    });

    // Load participants on page load
    loadParticipants();
});