import { dirname, normalize, resolve } from "node:path";
import { fileURLToPath } from "node:url";
import { defineConfig } from "vite";
import dts from "vite-plugin-dts";

const root = dirname(fileURLToPath(import.meta.url));
const srcRoot = normalize(resolve(root, "src"));

function isExternalModuleId(id: string): boolean {
  if (id.startsWith("\0")) return false;
  if (id.startsWith(".") || id.startsWith("/")) return false;
  if (/^[A-Za-z]:[/\\]/.test(id)) {
    const n = normalize(id);
    if (n.startsWith(srcRoot)) return false;
    return true;
  }
  return true;
}

export default defineConfig({
  plugins: [
    dts({
      tsconfigPath: "./tsconfig.lib.json",
      logLevel: "error",
    }),
  ],
  build: {
    emptyOutDir: true,
    outDir: "dist",
    sourcemap: true,
    lib: {
      entry: {
        index: resolve(root, "src/index.ts"),
        "ag-ui": resolve(root, "src/ag-ui.ts"),
      },
      formats: ["es", "cjs"],
      fileName: (format, entryName) =>
        `${entryName}.${format === "es" ? "js" : "cjs"}`,
    },
    rollupOptions: {
      external: (id) => isExternalModuleId(id),
    },
  },
});
