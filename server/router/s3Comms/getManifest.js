var s3 = require('../data/S3Object'),
    manifestName = require('../data/manifestName'),
    q = require('q');

module.exports = function(bucket) {
    var deferObj = q.defer();

    var getList = function(err, data) {

        if (err) {
            console.log(err.code);
            if (err.code == "NoSuchKey" || err.code == "NoSuchBucket") {
                deferObj.resolve(require('../data/blankManifest'));
            } else
                deferObj.reject(err);
        } else {
            deferObj.resolve(JSON.parse(data.Body));
        }
    };

    s3.get(manifestName, bucket, getList);

    return deferObj.promise;
};