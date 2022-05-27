(function () {
    'use strict';

    angular.
        module('clientApp').
        component('artistAdd', {

        controller: ArtistAddController,

        templateUrl: '/components/artist-add/artist-add.template.html'
        })

    function ArtistAddController(artistService, $window) {
        var self = this;
        self.artist = {
            name: null,
            description: null,
            facebookLink: null
        };

        self.createArtist = function () {
            artistService.create(self.artist).then(function (response) {
                self.artist = {
                    name: null,
                    description: null,
                    facebookLink: null
                };
                // redirect to /artist/index
                $window.location.href = "/artist";
            }),
            function () {
                console.log(response);
            }
        }
    }
})();