# Git Branch Management for Automation Scripts

This repository contains automation scripts that were previously hardcoded to push to the `main` branch. This created issues when:

1. The `main` branch was deleted from the remote (`git push origin --delete main`)
2. Working on feature branches where pushing to `main` would fail
3. Repository structure changes that affect branch availability

## Solution

The solution implements a `git-helper.sh` library that provides:

### Safe Push Function

```bash
safe_push [branch_name]
```

- **Detects branch existence**: Checks if target branch exists on remote before attempting push
- **Provides fallback options**: Suggests alternatives when target branch is missing
- **Branch mismatch warnings**: Alerts when current branch differs from target
- **Error handling**: Graceful failure with helpful error messages

### Updated Scripts

The following scripts have been updated to use the safe push functionality:

- `update-main.sh` - Main update automation script
- `createWeek.sh` - Weekly content creation script  
- `move-ghpages-to-main.sh` - Content migration script
- `update-be128.sh` - BE128 course content update script

### Usage

All scripts now automatically:

1. Load the `git-helper.sh` library
2. Use `safe_push` instead of direct `git push origin main`
3. Provide fallback to current branch if main branch is unavailable
4. Display informative messages about branch operations

### Handling Main Branch Deletion

When `git push origin --delete main` is executed:

1. **Before**: Scripts would fail with `error: src refspec main does not match any`
2. **After**: Scripts detect missing main branch and offer alternatives:
   - Push to current branch instead
   - Create new main branch
   - Skip push operation

### Backward Compatibility

- Scripts work normally when main branch exists
- Graceful degradation when git-helper.sh is not available
- No changes to script interfaces or parameters

### Testing

The solution has been tested for:

- ✅ Normal operation with existing main branch
- ✅ Operation when main branch is deleted
- ✅ Branch mismatch scenarios
- ✅ Error handling and user feedback
- ✅ Fallback to current branch operations