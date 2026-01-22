# 현재 저장소 상태 및 권장 작업 방법

## 현재 상태 요약

**현재 브랜치**: `copilot/create-feature-branch-from-main`  
**원격 저장소**: `origin` (https://github.com/JJH0204/25Machina)  
**upstream 설정**: 없음 (origin만 사용)

## 이 저장소에서 Main 브랜치 기반 작업하기

### 상황 1: 새로운 기능 개발 시작

```bash
# 1. 최신 원격 정보 가져오기
git fetch origin

# 2. 현재 작업 상태 확인
git status

# 3. main 브랜치가 로컬에 없다면 체크아웃
git checkout main
# 또는 origin/main을 기준으로 새 브랜치 바로 생성

# 4. origin/main 기준으로 새 브랜치 생성
git checkout -b feature/my-new-feature origin/main

# 5. 작업 수행...
# 파일 수정, 추가 등

# 6. 변경사항 커밋
git add .
git commit -m "feat: 새로운 기능 추가"

# 7. 원격에 푸시
git push -u origin feature/my-new-feature
```

### 상황 2: 현재 브랜치(copilot/create-feature-branch-from-main)를 계속 작업

현재 브랜치가 이미 생성되어 있고 작업 중이므로, 그대로 작업을 계속하시면 됩니다:

```bash
# 1. 현재 상태 확인
git status

# 2. 필요한 작업 수행

# 3. 변경사항 커밋
git add .
git commit -m "docs: Git workflow documentation added"

# 4. 원격에 푸시
git push origin copilot/create-feature-branch-from-main
```

### 상황 3: 작업 중 다른 긴급 작업 필요

```bash
# 1. 현재 작업 임시 저장
git stash push -m "WIP: 현재 문서 작업"

# 2. origin/main 기준으로 새 브랜치 생성
git fetch origin
git checkout -b hotfix/urgent-fix origin/main

# 3. 긴급 작업 수행 및 커밋
# ... 작업 ...
git add .
git commit -m "fix: 긴급 수정"
git push -u origin hotfix/urgent-fix

# 4. 원래 브랜치로 돌아가기
git checkout copilot/create-feature-branch-from-main

# 5. 임시 저장한 작업 복원
git stash pop
```

## 이 저장소의 특징

### 1. Main 브랜치가 로컬에 없음
현재 로컬에는 `copilot/create-feature-branch-from-main` 브랜치만 있습니다.  
새 작업을 시작할 때는 `origin/main`을 직접 참조하여 브랜치를 생성하세요.

```bash
# Main 브랜치를 로컬에 체크아웃하려면:
git checkout -b main origin/main
# 또는
git checkout main  # 원격에 main이 있으면 자동으로 추적
```

### 2. Upstream이 설정되지 않음
이 저장소는 fork가 아니므로 `origin`만 사용합니다.  
모든 가이드 문서에서 `upstream`이 나오면 `origin`으로 대체하세요.

### 3. Unity 프로젝트
이 저장소는 Unity 게임 프로젝트이므로:
- `.gitignore`가 이미 설정되어 있어 불필요한 파일은 자동으로 제외됩니다
- `Branch/` 디렉터리가 Unity 프로젝트 루트입니다
- 서브모듈(`ExternalAssets`)이 포함되어 있습니다

## 서브모듈 작업 시 주의사항

이 저장소는 `Branch/Assets/ExternalAssets` 서브모듈을 포함합니다:

```bash
# 서브모듈 초기화 및 업데이트
git submodule update --init --recursive

# 서브모듈의 최신 변경사항 가져오기
git submodule update --remote

# 서브모듈 상태 확인
git submodule status
```

## 추가 자료

- [Git 워크플로우 가이드](git-workflow-guide.md) - 전체 시나리오 및 상세 설명
- [Git 빠른 참조](git-quick-reference.md) - 자주 사용하는 명령어

## 현재 저장소에서 바로 사용 가능한 명령어

### 현재 상태 전체 확인
```bash
echo "=== 현재 브랜치 ==="
git rev-parse --abbrev-ref HEAD

echo "=== 원격 저장소 ==="
git remote -v

echo "=== 브랜치 목록 ==="
git branch -vv

echo "=== 워킹 디렉터리 상태 ==="
git status

echo "=== 최근 커밋 ==="
git log --oneline --graph --decorate -n 5
```

### Origin/main 기준으로 새 브랜치 생성 (복사해서 사용)
```bash
# 브랜치명을 원하는 이름으로 변경하세요
BRANCH_NAME="feature/my-feature"

git fetch origin
git checkout -b $BRANCH_NAME origin/main
echo "새 브랜치 '$BRANCH_NAME' 생성 완료!"
git status
```
