(function () {
    'use strict';

    angular.module('artist.service', [])
        .factory('artistService', [
            '$http',
            function ($http) {
                var apiUrl = '/artist';
                var requestConfig = {
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    }

                return {
                    // get all artists
                    getList: function () {
                        // returns promise
                        return $http.get(`${apiUrl}/list`);
                    },

                    // get artist by ID
                    getById: function (id) {
                        return $http.get(`${apiUrl}/${id}`);
                    },

                    // create artist
                    create: function (artistData) {
                        return $http({
                            url: `${apiUrl}/create`,
                            method: 'POST',
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            data: JSON.stringify(artistData)
                        });
                    },

                    // update artist by ID
                    update: function (artistId, artistData) {
                        return $http.post(`${apiUrl}/edit/${artistId}`, artistData, requestConfig);
                    },

                    // delete artist
                    delete: function (id) {
                        return $http.get(`${apiUrl}/delete/${id}`);
                    },
                }
            }
        ])
    
})();