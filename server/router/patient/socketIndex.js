var socketLookup = require('../utility/socketLookup');


module.exports = function(socket, io) {
    socket.on('patient:connected', function(data) {
        socketLookup.newPatient(socket);
        console.log("patient platform:" + data.platform);
        socketLookup.setPlatform(socket, data.platform);
        socket.emit('patient:connected');
    });

    // socket.on("patient:sendPatientView", function(data)
    // {
    //     console.log("sending patient View");
    //     var caregiverSocket = socketLookup.getCaregiverByPatient(socket);
    //     caregiverSocket.emit("caregiver:receivePatientView", data);
    // });

    socket.on("patient:sendPositionView", function(data) {
        console.log("sending position View");
        var caregiverSocket = socketLookup.getCaregiverByPatient(socket);
        caregiverSocket.emit("caregiver:receivePositionView", data);
    });


};