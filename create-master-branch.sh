#!/bin/bash
# Script to create a master branch from the main branch
# Implements the exact commands from the problem statement:
#   1. git checkout main
#   2. git checkout -b master
#   3. git push origin master
#
# This script includes error handling and checks for existing branches
set -euo pipefail

echo -e "\e[36m🔄 Creating master branch from main...\e[0m"

# Verify we are in a git repository
if ! git rev-parse --git-dir > /dev/null 2>&1; then
  echo -e "\e[31m❌ Not in a git repository\e[0m"
  exit 1
fi

# Step 1: Checkout main branch
if ! git rev-parse --verify main >/dev/null 2>&1; then
  echo -e "\e[31m❌ Main branch does not exist\e[0m"
  exit 1
fi
git checkout main
echo -e "\e[32m✅ Switched to main branch\e[0m"

# Step 2: Create and checkout master branch
if git rev-parse --verify master >/dev/null 2>&1; then
  echo -e "\e[33m⚠️  Master branch already exists, switching to it\e[0m"
  git checkout master
else
  echo -e "\e[34m📝 Creating new master branch\e[0m"
  git checkout -b master
fi
echo -e "\e[32m✅ Switched to master branch\e[0m"

# Step 3: Push master branch to origin
git push origin master
echo -e "\e[32m✅ Pushed master branch to origin\e[0m"

echo -e "\e[32m🎉 Master branch successfully created and pushed!\e[0m"