angular.module('backoffice')
  .filter('reais', reais);

reais.$inject = ["$window"]

function reais($window) {
  return function(x) {
    return $window.numeral(x).format('$0,0.00');
  }
}