var socketLookup = require('../utility/socketLookup'),
    getManifest = require('../s3Comms/getManifest'),
    s3 = require('../data/S3Object'),
    experienceBucketName = require('../data/s3ExperienceBucketSuffix'),
    animationBucketName = require('../data/s3AnimationBucketSuffix');

module.exports = function(socket, io) {

    socket.on('caregiver:connected', function(data) {
        if (data.platform == 'WindowsPlayer')
            data.platform = 'StandaloneWindows'
        socketLookup.setPlatform(socket, data.platform);
        console.log("caregiver platform: " + data.platform);
        socketLookup.pairPatient(data.patientID, socket, io);
        socket.emit('caregiver:paired');
        socketLookup.getPatientByCaregiver(socket).emit('patient:paired');
    });

    socket.on('caregiver:getContentInfo', function() {
        var platform = socketLookup.getPatientPlatformByCaregiver(socket);
        console.log("requesting content manifest from platform: " + socketLookup.getPatientPlatformByCaregiver(socket));
        //var platform = "Android";
        getManifest(platform + experienceBucketName).then(
            function(result) {
                socket.emit('caregiver:experienceManifest', result);
            },
            function(err) { socket.emit('error', err); });
    });

    socket.on('caregiver:selectContent', function(data) {
        var platform = socketLookup.getPatientPlatformByCaregiver(socket);
        //var platform = "Android";
        console.log("requesting content from platform: " + socketLookup.getPatientPlatformByCaregiver(socket));
        s3.getSignedURL(data.contentID, platform + experienceBucketName, function(err, url) {
            if (err)
                socket.emit('err', err);
            else {
                data.contentURL = url.url;
                socketLookup.getPatientByCaregiver(socket).emit('patient:contentURL', data);
            }
        }, function(err) { socket.emit('error', err); });
    });

    socket.on('caregiver:requestThumbnail', function(data) {
        var platform = socketLookup.getPatientPlatformByCaregiver(socket);
        //var platform = "Android";
        console.log("requesting thumbnail from platform: " + socketLookup.getPatientPlatformByCaregiver(socket));
        s3.getSignedURL(data.thumbnailID, platform + experienceBucketName, function(err, url) {
            if (err)
                socket.emit('err', err);
            else {
                data.url = url.url;
                socket.emit('caregiver:thumbnailURL', data);
            }
        });
    });

    socket.on('caregiver:toggleOffPositionView', function() {
        socketLookup.getPatientByCaregiver(socket).emit('patient:toggleOffPositionView');
    });

    socket.on('caregiver:toggleOnPositionView', function() {
        socketLookup.getPatientByCaregiver(socket).emit('patient:toggleOnPositionView');
    });

    socket.on('caregiver:movePositionView', function(data) {
        socketLookup.getPatientByCaregiver(socket).emit('patient:movePositionView', data);
    });

    socket.on('caregiver:selectPosition', function() {
        socketLookup.getPatientByCaregiver(socket).emit('patient:setPositionView');
    });

    socket.on('caregiver:sendAvatar', function(data) {
        socketLookup.getPatientByCaregiver(socket).emit('patient:receiveAvatar', data);
    });

    socket.on('caregiver:sendAvatarDNA', function(data) {
        socketLookup.getPatientByCaregiver(socket).emit('patient:receiveAvatarDNA', data);
    });
    socket.on('caregiver:sendAvatarColors', function(data) {
        socketLookup.getPatientByCaregiver(socket).emit('patient:receiveAvatarColors', data);
    });

    socket.on('caregiver:getAnimationLibrary', function() {

        var platform = socketLookup.getPlatform(socket);
        console.log("getting anim lib of platform: " + socketLookup.getPlatform(socket));
        //var platform = "StandaloneWindows";

        getManifest(platform + animationBucketName).then(
            function(result) {
                socket.emit('caregiver:receiveAnimationLibrary', result);
            }
        );
        //var platform = socketLookup.getPatientPlatformByCaregiver(socket);
        // var platform = "Android";
        // animationS3.listAllObjects(platform, function(error, data) {
        //     if (error)
        //         socket.emit('caregiver:error', error);
        //     else {
        //         var animLib = {};
        //         animLib.animNum = data.KeyCount;
        //         for (var i = 0; i < animLib.animNum; i++) {
        //             animLib['anim' + i] = data.Contents[i].Key;

        //         }
        //         socket.emit('caregiver:receiveAnimationLibrary', animLib);
        //     }
        // });

    });

    socket.on('caregiver:downloadAnimation', function(data) {
        var platform = socketLookup.getPlatform(socket);
        console.log("getting anim of platform: " + socketLookup.getPlatform(socket));
        //var platform = "StandaloneWindows";
        var animData = {}
        s3.getSignedURL(data.anim, platform + animationBucketName,
            function(error, urlData) {
                if (error == null) {
                    animData.url = urlData.url;
                    animData.animName = data.anim;
                    socket.emit('caregiver:downloadAnimURL', animData);
                } else
                    socket.emit("error", error);
            }, 0);
    });

    socket.on('caregiver:sendAnimations', function(data) {
        var anims = data.animNum;
        var count = 0;
        var platform = socketLookup.getPatientPlatformByCaregiver(socket);
        //var platform = "Android";
        console.log("sending anims of platform: " + socketLookup.getPatientPlatformByCaregiver(socket));

        for (var i = 0; i < anims; i++) {
            console.log("downloading anim : " + data['anim' + i]);
            s3.getSignedURL(data['anim' + i], platform + animationBucketName,
                function(error, urlData) {
                    data['anim' + urlData.animIndex + 'url'] = urlData.url;
                    count++;
                    if (count >= anims)
                        socketLookup.getPatientByCaregiver(socket).emit('patient:receiveAnimations', data);
                }, i);
        }
    });
};