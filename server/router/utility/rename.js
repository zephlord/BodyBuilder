var s3 = require('../data/S3Object'),
    q = require('q');

module.exports = function(bucketName, oldKey, newKey) {
    var deferObj = q.defer();
    if (oldKey == newKey)
        deferObj.resolve();
    else {
        s3.copyObject(bucketName, bucketName, oldKey, newKey, function(err, result) {
            if (err)
                deferObj.reject(err);
            else
                s3.delete(bucketName, oldKey, function(error, result) {
                    if (error)
                        deferObj.reject(error);
                    else
                        deferObj.resolve();
                });
        });
    }
    return deferObj.promise;
}