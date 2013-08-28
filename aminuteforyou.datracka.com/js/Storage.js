/**
 * Created with JetBrains PhpStorm.
 * User: vfayos
 * Date: 01.02.13
 * Time: 16:58
 * To change this template use File | Settings | File Templates.
 */

/**
 * Storage
 */

var Storage = function () {

    this.storage = {};
}

Storage.prototype.get = function (key) {
    return this.storage[key];
};

Storage.prototype.set = function (key, value) {
    this.storage[key] = value;
};
