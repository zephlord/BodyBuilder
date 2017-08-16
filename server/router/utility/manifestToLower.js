var q = require('q'),
    getManifest = require('../s3Comms/getManifest'),
    postManifest = require('../s3Comms/postManifest');

module.exports = function(bucketName) {
    var deferObj = q.defer();
    getManifest(bucketName).then(
        function(platformList) {
            for (var allTagIndex in platformList.allTags) {
                var tag = platformList.allTags[allTagIndex];
                var lowerCaseList = [];
                for (var i in platformList.tags[tag]) {
                    lowerCaseList.push(platformList.tags[tag][i].toLowerCase());
                }
                platformList.tags[tag] = lowerCaseList;
            }
            postManifest(platformList, bucketName).then(
                function() {
                    deferObj.resolve("complete");
                },
                function(error) { deferObj.reject(error); }
            );
        },
        function(err) { deferObj.reject(err); }
    );

    return deferObj.promise;
}