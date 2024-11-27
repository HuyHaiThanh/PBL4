chrome.downloads.onCreated.addListener((downloadItem) => {
    console.log("Download detected: ", downloadItem.url);

    // Gửi link tới ứng dụng C# qua HTTP
    fetch("http://localhost:5000/catch", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ url: downloadItem.url })
    }).then(() => {
        console.log("Download link sent to C# application.");
    }).catch((error) => {
        console.error("Failed to send link: ", error);
    });
});
