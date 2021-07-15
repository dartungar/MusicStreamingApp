(function () {
    'use strict';

    angular.module('artist.service', [])
        .factory('artistService', [
            '$http',
            function ($http) {
                var apiUrl = '/artist';

                return {
                    getList: function () {
                        // returns promise
                        return $http.get(`${apiUrl}/list`);
                    }
                }
            }
        ])
    
})();