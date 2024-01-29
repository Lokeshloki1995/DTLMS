

$(function () {

  /**
   * This is a quick example of capturing the select event on tree leaves, not branches
   * (We're going to work on this a bit)
   */
  $("body").on("nodeselect.tree.data-api", "[role=leaf]", function (e) {
    
    var output = "<p>Node <b>nodeselect</b> event fired:<br>"
      + "Node Type: leaf<br>"
      + "Value: " + ((e.node.value) ? e.node.value : e.node.el.text()) + "<br>"
      + "Parentage: " + e.node.parentage.join("/") + "</p>"
      
      $('div#reporter').prepend(output)
    
  })
  
  /**
   * This is a quick example of capturing the select event on tree branches, not leaves
   * (We're going to work on this a bit)
   */
  $("body").on("nodeselect.tree.data-api", "[role=branch]", function (e) {
    
    var output = "<p>Node <b>nodeselect</b> event fired:<br>"
      + "Node Type: branch<br>"
      + "Value: " + ((e.node.value) ? e.node.value : e.node.el.text()) + "<br>"
      + "Parentage: " + e.node.parentage.join("/") + "</p>"
      
      $('div#reporter').prepend(output)
    
  })
  
  /**
   * Listening for the 'openbranch' event. Look for e.node, which is the
   * actual node the user opens
   */
  $("body").on("openbranch.tree", "[data-toggle=branch]", function (e) {
    
    var output = "<p>Node <b>openbranch</b> event fired:<br>"
      + "Node Type: branch<br>"
      + "Value: " + ((e.node.value) ? e.node.value : e.node.el.text()) + "<br>"
      + "Parentage: " + e.node.parentage.join("/") + "</p>"
      
      $('div#reporter').prepend(output)
      
  })
  
  /**
   * Listening for the 'closebranch' event. Look for e.node, which is the
   * actual node the user closed
   */
  $("body").on("closebranch.tree", "[data-toggle=branch]", function (e) {
    
    var output = "<p>Node <b>closebranch</b> event fired:<br>"
      + "Node Type: branch<br>"
      + "Value: " + ((e.node.value) ? e.node.value : e.node.el.text()) + "<br>"
      + "Parentage: " + e.node.parentage.join("/") + "</p>"
      
      $('div#reporter').prepend(output)
    
  })

})

var cbExample = function (response, status, xhr) {
  console.log("this ", this)
  console.log("data", arguments)
}