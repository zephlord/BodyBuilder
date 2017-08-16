var q = require('q'),
    guid = require('../utility/guidGen');


module.exports = function(fileName, info, timestamp){
    var deferObj = q.defer();
    var crypto = require('crypto'),
    hash = crypto.createHash('sha256');
    var plainTextKey = JSON.stringify({name:fileName, info: info, timestamp: timestamp}) + guid();
    hash.update(plainTextKey);
    var result = hash.digest('hex');
    result.replace(/[&\/\\#@,+()$~%.'":*?<>{}| ]/g, '');

    return result;
};