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
        sassPlugin({
            loadPaths: [
                '.',
                'node_modules/@ministryofjustice/frontend',
                'node_modules/govuk-frontend/dist',
            ],
            quietDeps: true
        })
    ],
    outfile: "out/css/application.css",
});
// -- Build CSS --

// -- Copy GOV.UK Assets --
const assetDirectoryGovUk = "./node_modules/govuk-frontend/dist/govuk/assets/";
const subFoldersGovUk = ["images", "fonts"];

for (const subFolder of subFoldersGovUk) {
    const targetDirectory = assetDirectoryGovUk + subFolder;
    readdirSync(targetDirectory).forEach(file => {
        cpSync(`${targetDirectory}/${file}`, `./out/assets/${subFolder}/${file}`, { force: true });
    });
}
// -- Copy GOV.UK Assets --

// -- Copy MoJ Assets --
const assetDirectoryMoJ = "./node_modules/@ministryofjustice/frontend/moj/assets/";
const subFoldersMoJ = ["images"];

for (const subFolder of subFoldersMoJ) {
    const targetDirectory = assetDirectoryMoJ + subFolder;
    readdirSync(targetDirectory).forEach(file => {
        cpSync(`${targetDirectory}/${file}`, `./out/assets/${subFolder}/${file}`, { force: true });
    });
}
// -- Copy MoJ Assets --

// -- Copy GOV.UK Minified JS --
cpSync("./node_modules/govuk-frontend/dist/govuk/govuk-frontend.min.js", "./out/js/govuk-frontend.min.js", { force: true });
cpSync("./node_modules/govuk-frontend/dist/govuk/govuk-frontend.min.js.map", "./out/js/govuk-frontend.min.js.map", { force: true });
// -- Copy GOV.UK Minified JS --

// -- Copy MoJ Minified JS --
cpSync("./node_modules/@ministryofjustice/frontend/moj/moj-frontend.min.js", "./out/js/moj-frontend.min.js", { force: true });
cpSync("./node_modules/@ministryofjustice/frontend/moj/moj-frontend.min.js.map", "./out/js/moj-frontend.min.js.map", { force: true });
// -- Copy MoJ Minified JS --

// - Copy Output to ServiceDirectory.Presentation.Web --
const outputDirectory = "../ServiceDirectory.Presentation.Web/wwwroot/";
const outputSubFolders = ["css", "assets", "js"];

for (const outputSubFolder of outputSubFolders) {
    const inputSourceDirectory = "./out/" + outputSubFolder;
    const outputTargetDirectory = outputDirectory + outputSubFolder;
    cpSync(inputSourceDirectory, outputTargetDirectory, { force: true, recursive: true });
}
// - Copy Output to ServiceDirectory.Presentation.Web --
