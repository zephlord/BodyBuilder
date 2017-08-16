var q = require('q'),
    postAnimation = require('./postAnimation'),
    getManifest = require('./getManifest'),
    postManifest = require('./postManifest'),
    includeCheck = require('../utility/includes'),
    animationBucketName = require('../data/s3AnimationBucketSuffix');


module.exports = function(fileInfo, platform) {
    var deferObj = q.defer();
    var uploadedSuccessfully = {};
    var errors = [];
    var count = 0;

    var modifyManifest = function(uploadedFiles, allErrors) {
        getManifest(platform + animationBucketName).then(
            function(fileList) {
                for (var fileName in uploadedFiles) {
                    for (var tagIndex in uploadedFiles[fileName]) {
                        var tag = uploadedFiles[fileName][tagIndex];
                        if (!includeCheck(fileList.allTags, tag)) {
                            fileList.tags[tag] = [fileName];
                            fileList.allTags.push(tag);
                        } else
                            fileList.tags[tag].push(fileName);
                    }
                }
                postManifest(fileList, platform + animationBucketName).then(
                    function() {
                        deferObj.resolve({ erors: allErrors, uploaded: uploadedFiles });
                    },
                    function(error) {
                        deferObj.reject(error);
                    });
            });
    }


    for (var animation in fileInfo) {
        var fileName = fileInfo[animation].fileName;
        var path = fileInfo[animation].path;
        var tags = fileInfo[animation].tags;
        var animNum = Object.keys(fileInfo).length;
        postAnimation(fileName, path, tags, platform).then(
            function(animationInfo) {
                uploadedSuccessfully[animationInfo.fileName] = animationInfo.tags;
                count++;
                if (count >= animNum)
                    modifyManifest(uploadedSuccessfully, errors);
            },
            function(error) {
                errors.push(error);
                count++;
                if (count >= animNum)
                    modifyManifest(uploadedSuccessfully, errors);
            }
        );

    }

    return deferObj.promise;
}