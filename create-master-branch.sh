#!/bin/bash
set -euo pipefail

echo -e "\e[36m🔄 Creating master branch from main...\e[0m"

# Step 1: Checkout main branch
git checkout main
echo -e "\e[32m✅ Switched to main branch\e[0m"

# Step 2: Create and checkout master branch
git checkout -b master
echo -e "\e[32m✅ Created and switched to master branch\e[0m"

# Step 3: Push master branch to origin
git push origin master
echo -e "\e[32m✅ Pushed master branch to origin\e[0m"

echo -e "\e[32m🎉 Master branch successfully created and pushed!\e[0m"