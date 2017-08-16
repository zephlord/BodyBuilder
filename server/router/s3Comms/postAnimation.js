var q = require('q'),
    animationBucketPrefix = require('../data/s3AnimationBucketSuffix'),
    s3 = require('../data/S3Object');



module.exports = function(fileName, filePath, tags, platform) {
    var deferObj = q.defer();


    s3.putFile(fileName, filePath, platform + animationBucketPrefix,
        function(error, data) {
            if (error)
                deferObj.reject(error);
            else
                deferObj.resolve({
                    fileName: fileName,
                    tags: tags
                });
        });
    return deferObj.promise;
};