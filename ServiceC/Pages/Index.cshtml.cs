@page
@model IndexModel

<h1>Redis Keys and Values</h1>
<ul id="keyList"></ul>

<script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
<script>
    $(document).ready(function () {
        // Function to get keys from the server and update the page
        function getKeys() {
            $.ajax({
                type: "GET",
                url: "/Index?handler=GetKeys",
                success: function (data) {
                    updateKeys(data);
                },
                error: function (error) {
                    console.error("Error getting keys: ", error);
                }
            });
        }

        // Function to update the HTML page with the retrieved keys and values
        function updateKeys(keyValuePairs) {
            var keyList = $("#keyList");

            // Remove keys that are no longer present in the server response
            keyList.children().each(function () {
                var keyElement = $(this);
                var key = keyElement.find("strong").text();
                if (!keyValuePairs.some(pair => pair.key === key)) {
                    keyElement.remove();
                }
            });

            // Add or update keys from the server response
            keyValuePairs.forEach(function (pair) {
                var existingKeyElement = keyList.find("strong:contains('" + pair.key + "')").closest("li");
                if (existingKeyElement.length > 0) {
                    // Update existing key
                    existingKeyElement.html("<strong>" + pair.key + ":</strong> " + pair.value);
                } else {
                    // Add new key
                    keyList.append("<li><strong>" + pair.key + ":</strong> " + pair.value + "</li>");
                }
            });
        }

        // Initial load of keys
        getKeys();

        // Set up a timer to refresh the keys every 5 seconds
        setInterval(function () {
            getKeys();
        }, 5000);
    });
</script>
