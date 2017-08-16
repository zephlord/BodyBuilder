var q = require('q'),
    makeS3Key = require('../utility/makeS3Key'),
    experienceBucketName = require('../data/s3ExperienceBucketSuffix'),
    newManifest = require('../data/blankManifest'),
    getFileList = require('./getManifest'),
    postManifest = require('./postManifest'),
    includeCheck = require('../utility/includes'),
    S3Object = require('../data/S3Object');



module.exports = function postExperience(tags, path, name, info, thumbnails, sceneName, platform) {
    var deferObj = q.defer();
    var timestamp = Date.now().toString();
    getFileList(platform + experienceBucketName).then(
        function(platformList) {

            var experienceKey = makeS3Key(name, info, timestamp)
            var experienceObj = {
                name: name,
                info: info,
                key: experienceKey,
                thumbnails: [],
                sceneName: sceneName,
                timestamp: timestamp
            };
            S3Object.putFile(experienceKey, path, platform + experienceBucketName,
                function(err, result) {
                    if (err)
                        deferObj.reject(err);
                    else {
                        var infoKey = makeS3Key(experienceKey + "info", experienceObj, timestamp);
                        for (var i = 0; i < tags.length; i++) {
                            if (platformList === null || platformList === undefined)
                                platformList = newManifest;

                            var tagList = platformList.tags[tags[i]];
                            if (tagList === null || tagList === undefined)
                                platformList.tags[tags[i]] = [infoKey];
                            else
                                platformList.tags[tags[i]].push(infoKey);
                            console.log("platformList " + platformList);
                            console.log("platformList allTags" + platformList.allTags);
                            if (platformList.allTags == null)
                                platformList.allTags = [];
                            if (!includeCheck(platformList.allTags, tags[i]))
                                platformList.allTags.push(tags[i]);
                        }
                        var targetThumbUpload = thumbnails.length;
                        var count = 0;
                        var errString = "";
                        var thumbKeys = [];
                        for (var i = 0; i < thumbnails.length; i++) {
                            thumbKeys.push(makeS3Key(thumbnails[i], "image", timestamp));
                            S3Object.put(thumbKeys[i], thumbnails[i], "image/png", platform + experienceBucketName,
                                function(err, result) {

                                    if (err)
                                        errString += "\n there was a problem uploading thumbnail " + count;
                                    else
                                        experienceObj.thumbnails.push(thumbKeys[count]);
                                    count++;
                                    if (count == targetThumbUpload) {
                                        platformList[infoKey] = experienceObj;
                                        postManifest(platformList, platform + experienceBucketName).then(
                                            function() {

                                                deferObj.resolve({
                                                    infoKey: infoKey,
                                                    experienceKey: experienceKey,
                                                    platform: platform,
                                                    error: errString
                                                });
                                            },
                                            function(err) { deferObj.reject(err); });
                                    }
                                },
                                function(err) { deferObj.reject(err); });
                        }
                    }
                },
                function(err) { deferObj.reject(err); });
        },
        function(err) { deferObj.reject(err); });

    return deferObj.promise;
};