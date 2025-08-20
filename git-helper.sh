#!/bin/bash
# Git Helper Library for handling branch operations safely
# This helps scripts work correctly even if main branch is deleted

# Function to get the current branch name
get_current_branch() {
    git branch --show-current 2>/dev/null || echo ""
}

# Function to check if a branch exists locally
branch_exists_local() {
    local branch_name="$1"
    git show-ref --verify --quiet "refs/heads/$branch_name" 2>/dev/null
}

# Function to check if a branch exists on remote
branch_exists_remote() {
    local branch_name="$1"
    git ls-remote --heads origin "$branch_name" 2>/dev/null | grep -q "refs/heads/$branch_name"
}

# Function to safely push to a branch
safe_push() {
    local target_branch="${1:-main}"
    local current_branch
    current_branch=$(get_current_branch)
    
    echo -e "\e[36m🔄 Attempting to push to $target_branch...\e[0m"
    
    # Check if target branch exists on remote
    if ! branch_exists_remote "$target_branch"; then
        echo -e "\e[31m❌ Remote branch '$target_branch' does not exist!\e[0m"
        echo -e "\e[33m💡 You may need to create the branch first or push to current branch '$current_branch'\e[0m"
        return 1
    fi
    
    # If we're not on the target branch, warn user
    if [ "$current_branch" != "$target_branch" ]; then
        echo -e "\e[33m⚠️  Warning: Current branch '$current_branch' differs from target '$target_branch'\e[0m"
        echo -e "\e[33m🔄 Pushing current branch to remote $target_branch...\e[0m"
    fi
    
    # Attempt the push with error handling
    if git push origin "HEAD:$target_branch"; then
        echo -e "\e[32m✅ Successfully pushed to $target_branch!\e[0m"
        return 0
    else
        echo -e "\e[31m❌ Failed to push to $target_branch\e[0m"
        echo -e "\e[33m💡 Consider pushing to current branch instead: git push origin $current_branch\e[0m"
        return 1
    fi
}

# Function to handle the case where main branch might be deleted
handle_missing_main() {
    echo -e "\e[31m❌ Main branch appears to be missing!\e[0m"
    echo -e "\e[36m🔄 Checking available options...\e[0m"
    
    local current_branch
    current_branch=$(get_current_branch)
    
    echo -e "\e[33m📝 Available options:\e[0m"
    echo -e "\e[33m  1. Push to current branch: $current_branch\e[0m"
    echo -e "\e[33m  2. Create and push new main branch\e[0m"
    echo -e "\e[33m  3. Skip push operation\e[0m"
    
    # For automation, we'll push to current branch as safest option
    echo -e "\e[36m🔄 Auto-selecting option 1: Push to current branch\e[0m"
    safe_push "$current_branch"
}

# Export functions for use in other scripts
export -f get_current_branch branch_exists_local branch_exists_remote safe_push handle_missing_main