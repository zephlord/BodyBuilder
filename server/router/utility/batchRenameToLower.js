var manifestName = require('../data/manifestName'),
    s3 = require('../data/S3Object'),
    rename = require('./rename'),
    redoManifest = require('./manifestToLower'),
    q = require('q');

module.exports = function(bucketName) {




    var deferObj = q.defer();
    s3.listAllObjects(bucketName, function(err, fileList) {
        if (err)
            deferObj.reject(err);
        else {
            var count = 0;
            for (var i = 0; i < fileList.length; i++) {
                if (fileList[i] == manifestName)
                    continue;
                else {
                    rename(bucketName, fileList[i], fileList[i].toLowerCase()).then(
                        function() {
                            count++;
                            if (count >= fileList.length) {
                                redoManifest(bucketName).then(function() {
                                    deferObj.resolve();
                                }, function(error) {
                                    deferObj.reject(error);
                                });
                            }
                        },
                        function(error) {
                            count++;
                            deferObj.reject(error);
                        });
                }
            }
        }
    });
    return deferObj.promise;
};