#!/bin/bash
# Simple local documentation viewer
# This script helps preview the generated documentation locally

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
DOCS_DIR="$REPO_ROOT/docs/events"

echo "Event Documentation Viewer"
echo "=========================="
echo ""
echo "Documentation Location: $DOCS_DIR"
echo ""
echo "Available Files:"
echo "  1. README.md - Overview and Architecture"
echo "  2. asyncapi.yaml - AsyncAPI 3.0 Specification"  
echo "  3. integration-events.md - CAP Integration Events"
echo "  4. domain-events.md - MediatR Domain Events"
echo ""
echo "To view AsyncAPI specification:"
echo "  - Online: https://studio.asyncapi.com/"
echo "  - Paste the content of: $DOCS_DIR/asyncapi.yaml"
echo ""
echo "To view Markdown files with diagrams:"
echo "  - VS Code: Install 'Markdown Preview Mermaid Support' extension"
echo "  - GitHub: Push to repository and view on GitHub"
echo "  - Obsidian: Open the docs folder in Obsidian"
echo ""

# Check if files exist
if [ ! -f "$DOCS_DIR/README.md" ]; then
    echo "❌ Documentation not generated yet!"
    echo "   Run: ./tools/generate-docs.sh"
    exit 1
fi

echo "✅ Documentation is available!"
echo ""
echo "File sizes:"
ls -lh "$DOCS_DIR"/*.md "$DOCS_DIR"/*.yaml | awk '{print "  " $9 ": " $5}'
echo ""
