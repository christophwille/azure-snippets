// https://azure.microsoft.com/en-us/blog/running-go-applications-on-azure-app-service/

package main

import (
    "fmt"
    "log"
    "net/http"
    "os"
)

func helloHandler(w http.ResponseWriter, r *http.Request) {
	msg := r.FormValue("msg")
	
	if msg == "" {
		msg = "friend"
	}

    fmt.Fprintln(w, "Hello, " + msg)
}

func main() {
	port := "8081"
    if os.Getenv("HTTP_PLATFORM_PORT") != "" {
        port = os.Getenv("HTTP_PLATFORM_PORT")
    }

    http.HandleFunc("/hello", helloHandler)
    err := http.ListenAndServe(":" + port, nil)
    if err != nil {
        log.Fatal(err)
    }
}