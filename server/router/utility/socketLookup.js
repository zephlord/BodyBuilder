// var HashTable = require('hashtable'),
//     patientSocketTable = new HashTable(),
//     patientCaregiverPair = new HashTable(),
//     patientPlatform = new HashTable();
var patientCaregiverPair = {},
    PlatformDict = {};

function SocketLookup() {
    this.newPatient = function(socket) {
        socket.nickname = socket.id;
        socket.join(socket.id);
    };

    this.setPlatform = function(patientSocket, platform) {
        PlatformDict[patientSocket.nickname] = platform;
        // patientPlatform.put(socket.id, platform);
    };

    this.getPatientPlatformByCaregiver = function(caregiverSocket) {
        return PlatformDict[patientCaregiverPair[caregiverSocket.nickname].nickname];
    };

    this.getPlatform = function(socket) {
        return PlatformDict[socket.nickname];
    }

    this.getPatientByCaregiver = function(caregiverSocket) {
        return patientCaregiverPair[caregiverSocket.nickname];
        //        return patientCaregiverPair.get(socket.id);
    };

    this.getCaregiverByPatient = this.getPatientByCaregiver;

    this.findPatient = function(patientID, io) {
        var sockets = io.sockets.adapter.rooms[patientID].sockets;
        for (var socketID in sockets) {
            if (sockets[socketID])
                return io.sockets.connected[socketID];
        }
        //        return patientSocketTable.get(patientID);
    };

    this.pairPatient = function(patientID, caregiverSocket, io) {
        var patientSocket = this.findPatient(patientID, io);
        // patientCaregiverPair.put(caregiverSocket.id, patientSocket);
        // patientCaregiverPair.put(patientSocket.id, caregiverSocket);
        patientCaregiverPair[caregiverSocket.nickname] = patientSocket;
        patientCaregiverPair[patientSocket.nickname] = caregiverSocket;
    };

    this.removePatient = function(patientSocket) {
        // var caregiverID = patientCaregiverPair.get(socket.id);
        // patientCaregiverPair.remove(socket.id);
        // patientCaregiverPair.remove(caregiverID);
        // patientSocketTable.remove(socket.id);
        var caregiverSocket = patientCaregiverPair[patientSocket.nickname];
        delete patientCaregiverPair[patientSocket.nickname];
        delete patientCaregiverPair[caregiverSocket.nickname];
        delete PlatformDict[patientSocket.nickname];

    };
}

module.exports = new SocketLookup();