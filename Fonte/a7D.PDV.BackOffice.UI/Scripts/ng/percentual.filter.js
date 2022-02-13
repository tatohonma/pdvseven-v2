angular.module('backoffice')
  .filter('percentual', percentual);

percentual.$inject = ["$window"]

function percentual($window) {
  return function(x) {
    return $window.numeral(x).format('0.00%')
  }
}