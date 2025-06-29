# Service Directory - Node

This project contains the GOV.UK frontend library as well as a bundle, minify and copy build system.

It must be run each time the GOV.UK frontend library is updated, or if any SCSS is added, modified or otherwise changed.

## Instructions

In a terminal..

### Prerequisites

1. Node.JS v22.16.0 or Later
2. NPM v10.9.2 or Later

### Running

1. Execute `npm i`
2. Execute `npm run build`

The program will then take the JS, CSS, and other assets of the GOV.UK frontend, bundle and minify them, and copy the result to the `wwwroot` folder located in the `ServiceDirectory.Presentation.Web` .NET project.
