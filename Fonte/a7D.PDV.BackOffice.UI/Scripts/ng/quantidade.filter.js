angular.module('backoffice')
  .filter('quantidade', quantidade);

function quantidade($window) {
  return function(x) {
    return $window.numeral(x).format('0,0');
  }
}