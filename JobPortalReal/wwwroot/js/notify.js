

//function updateBookingRequestNotificationCount(userId) {
//    $.ajax({
//        url: `https://localhost:7238/api/Notification/markasread/Count/${userId}`, // API URL with userId
//        type: "GET",
//        success: function (response) {
//            console.log(response);
//            // Assuming your API returns { success: true, count: X }
//            if (response.success) {
//                const count = response.count;
//                console.log("hiii");
//                // Update the badge
//                const badge = $(".icon-button__badge");
//                badge.text(count);

//                // Show or hide the badge based on count
//                if (count === 0) {
//                    badge.addClass("visually-hidden");
//                } else {
//                    badge.removeClass("visually-hidden");
//                }
//            }
//        },
//        error: function () {
//            console.error("Failed to fetch booking request notification count.");
//        }
//    });
//}

//// Fetch the userId from the API and then call the function
//$.ajax({
//    url: "https://localhost:7238/api/Notification/getUserId",
//    type: "GET",
//    success: function (response) {
//        if (response.success) {
//            const userId = response.id;
//            updateBookingRequestNotificationCount(userId);

//            // Update the badge every 10 seconds
//            setInterval(function () {
//                updateBookingRequestNotificationCount(userId);
//            }, 10000);
//        }
//    },
//    error: function () {
//        console.error("Failed to fetch userId.");
//    }
//});