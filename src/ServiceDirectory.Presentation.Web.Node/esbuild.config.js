import * as esbuild from "esbuild";
import { sassPlugin } from "esbuild-sass-plugin";
import { cpSync, readdirSync } from "fs";

// -- Build CSS --
await esbuild.build({
    entryPoints: [
        "application.scss"
    ],
    bundle: true,
    minify: true,
    sourcemap: true,
    external: [
        "/assets/*"
    ],
    plugins: [
        sassPlugin()
    ],
    outfile: "out/css/application.css",
});
// -- Build CSS --

// -- Copy GOV.UK Assets --
const assetDirectory = "./node_modules/govuk-frontend/dist/govuk/assets/";
const subFolders = ["images", "fonts"];

for (const subFolder of subFolders) {
    const targetDirectory = assetDirectory + subFolder;
    readdirSync(targetDirectory).forEach(file => {
        cpSync(`${targetDirectory}/${file}`, `./out/assets/${subFolder}/${file}`, { force: true });
    });
}
// -- Copy GOV.UK Assets --

// -- Copy GOV.UK Minified JS --
cpSync("./node_modules/govuk-frontend/dist/govuk/govuk-frontend.min.js", "./out/js/govuk-frontend.min.js", { force: true });
cpSync("./node_modules/govuk-frontend/dist/govuk/govuk-frontend.min.js.map", "./out/js/govuk-frontend.min.js.map", { force: true });
// -- Copy GOV.UK Minified JS --

// - Copy Output to ServiceDirectory.Presentation.Web --
const outputDirectory = "../ServiceDirectory.Presentation.Web/wwwroot/";
const outputSubFolders = ["css", "assets", "js"];

for (const outputSubFolder of outputSubFolders) {
    const inputSourceDirectory = "./out/" + outputSubFolder;
    const outputTargetDirectory = outputDirectory + outputSubFolder;
    cpSync(inputSourceDirectory, outputTargetDirectory, { force: true, recursive: true });
}
// - Copy Output to ServiceDirectory.Presentation.Web --
