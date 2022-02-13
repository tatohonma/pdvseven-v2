;(function () {
  'use strict'
  angular
    .module('AtivacaoApp')
    .filter('removerEspacos', [ function () {
      return function (string) {
        if (!angular.isString(string)) {
          return string
        }
        return string.replace(/[\s]/g, '')
      }
    }])
})()
