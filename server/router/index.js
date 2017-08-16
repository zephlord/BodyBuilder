var patientSocket = require('./patient/socketIndex'),
    caregiverSocket = require('./caregiver/socketIndex'),
    formidable = require('formidable'),
    postExperience = require('./s3Comms/postExperience'),
    postAnimations = require('./s3Comms/postAnimations'),
    s3 = require('./data/S3Object');



module.exports = function(app, io) {
    io.on('connection', function(socket) {
        patientSocket(socket, io);
        caregiverSocket(socket, io);

    });

    app.post('/experience', function(req, res) {
        var form = new formidable.IncomingForm();
        form.parse(req, function(error, fields, files) {

            var fileName = fields.fileName;
            var tags = JSON.parse(fields.tags).tags;
            var info = fields.info;
            var sceneName = fields.sceneName;
            var platform = fields.platformName;
            var thumbnails = [];
            for (var i = 0; i < parseInt(fields.thumbNum); i++)
                thumbnails.push(files["thumbnail" + i].path);

            postExperience(tags, files["bundle"].path, fileName, info, thumbnails, sceneName, platform).then(
                function(keys) {
                    res.send({
                        contentKey: keys.experienceKey,
                        infoKey: keys.infoKey,
                        platform: keys.platform
                    });
                },
                function(err) { res.send({ error: err }); });
        });
    });

    app.post('/animation', function(req, res) {
        var form = new formidable.IncomingForm();
        form.parse(req, function(error, fields, files) {

            var animNum = parseInt(fields.animNum);
            var platforms = JSON.parse(fields.platformName).platforms;
            var fileInfo = {};

            for (platform in platforms) {
                for (var i = 0; i < animNum; i++) {

                    if (fields['bundleName' + i] == undefined)
                        continue;
                    var fileName = fields['bundleName' + i].toLowerCase();
                    var tags = JSON.parse(fields['tags' + i]).tags;
                    var path = files[platforms[platform] + '-bundle' + i].path;
                    var obj = {
                        fileName: fileName,
                        tags: tags,
                        path: path
                    };
                    fileInfo[fileName] = obj;
                }
                postAnimations(fileInfo, platforms[platform]).then(
                    function(data) {
                        res.send(data);
                    },
                    function(error) {
                        res.send(error);
                    });
            }

        }, function(error) {
            res.send(error);
        });

    });


    var renameToLower = require('./utility/batchRenameToLower');
    var animationBucketSuffix = require('./data/s3AnimationBucketSuffix');
    app.get('/toLowerAnimFiles', function(req, res) {
        var folders = ['Android', 'StandaloneWindows', 'StandaloneWindows64'];
        for (var i = 0; i < folders.length; i++) {
            renameToLower(folders[i] + animationBucketSuffix).then(function(err, result) {
                if (err)
                    res.send(err);
                else
                    res.send({ result: 'success' });
            });
        }
    });

    var redoManifest = require('./utility/manifestToLower');
    app.get('/toLowerManifest', function(req, res) {
        var folders = ['Android', 'StandaloneWindows', 'StandaloneWindows64'];
        for (var i = 0; i < folders.length; i++) {
            redoManifest(folders[i] + animationBucketSuffix).then(function() {
                res.send({ result: 'success' });
            }, function(error) { res.send(error); });
        }
    });


};