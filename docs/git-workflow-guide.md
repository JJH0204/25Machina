# Main 브랜치 기반 안전한 작업 가이드

## 목차
1. [개요](#개요)
2. [사전 확인 사항](#사전-확인-사항)
3. [시나리오별 작업 가이드](#시나리오별-작업-가이드)
4. [안전 수칙 및 복구 방법](#안전-수칙-및-복구-방법)

---

## 개요

이 문서는 로컬 main 브랜치를 원격(upstream 또는 origin)의 최신 상태로 유지하고,  
main을 기준으로 안전하게 작업(커밋, 브랜치 생성, rebase 등)을 수행하는 방법을 정리합니다.

### 주요 원칙
- **백업 우선**: 위험한 작업 전에는 항상 백업 브랜치를 생성합니다
- **검증 후 진행**: 각 단계마다 상태를 확인하고 진행합니다
- **force-with-lease 사용**: 강제 푸시 시 `--force-with-lease`를 사용하여 안전성을 높입니다
- **협업 고려**: 공유 브랜치에서 히스토리 변경 작업은 팀원과 사전 합의합니다

---

## 사전 확인 사항

모든 작업 전에 다음 명령어들로 현재 상태를 확인하세요:

```bash
# 1. 현재 브랜치 확인
git rev-parse --abbrev-ref HEAD

# 2. 원격 저장소 확인
git remote -v

# 3. 브랜치 목록 및 추적 상태 확인
git branch -vv

# 4. 워킹 디렉터리 상태 확인
git status

# 5. 최신 원격 정보 가져오기
git fetch --all --prune

# 6. 커밋 히스토리 확인
git log --oneline --graph --decorate -n 10
```

### 원격 저장소 설정 확인

```bash
# origin만 있는 경우 (일반적인 경우)
git remote -v
# origin https://github.com/user/repo (fetch)
# origin https://github.com/user/repo (push)

# upstream이 별도로 설정된 경우 (fork한 저장소)
# origin  https://github.com/myuser/repo (fetch)
# origin  https://github.com/myuser/repo (push)
# upstream https://github.com/original/repo (fetch)
# upstream https://github.com/original/repo (push)
```

**참고**: 이후 명령어에서 `upstream`을 사용하는 경우, 
upstream이 없다면 `origin`으로 대체하세요.

---

## 시나리오별 작업 가이드

### 시나리오 A: Main 최신 상태에서 새 브랜치 생성 및 커밋

**목적**: 원격의 최신 main을 기준으로 새로운 기능 브랜치를 만들어 작업

**전제조건**:
- 현재 작업 중인 변경사항이 없거나 커밋됨
- 새로운 기능 개발을 시작하려는 경우

**단계**:

```bash
# 1. 최신 원격 정보 가져오기
git fetch origin  # 또는 git fetch upstream

# 2. 워킹 디렉터리가 깨끗한지 확인
git status

# 3. 원격의 최신 main(또는 HEAD)를 기준으로 새 브랜치 생성
git checkout -b feature/my-new-feature origin/main
# upstream이 있는 경우: git checkout -b feature/my-new-feature upstream/main

# 4. 작업 수행 (파일 수정 등)

# 5. 변경사항 스테이징
git add .

# 6. 커밋
git commit -m "feat: 새로운 기능 추가"

# 7. 원격에 푸시 (새 브랜치 생성)
git push -u origin feature/my-new-feature
```

**검증**:
```bash
# 현재 브랜치 확인
git branch -vv

# 원격과의 차이 확인
git log --oneline --graph origin/main..HEAD
```

---

### 시나리오 B: 기존 작업 브랜치를 Main 최신 위로 재배치 (Rebase)

**목적**: 이미 작업 중인 브랜치의 커밋들을 최신 main 위로 옮기기

**전제조건**:
- 기존 작업 브랜치가 있음
- 해당 브랜치의 커밋을 보존하고 싶음
- main의 최신 변경사항을 반영하고 싶음

**주의사항**:
- Rebase는 커밋 히스토리를 변경하므로, 이미 푸시한 브랜치는 force push가 필요
- 다른 사람과 공유하는 브랜치는 사전 합의 필요

**단계**:

```bash
# 1. 작업 브랜치로 이동
git checkout my-work-branch

# 2. 백업 브랜치 생성 (필수!)
git branch backup/my-work-branch-before-rebase

# 3. 최신 원격 정보 가져오기
git fetch origin  # 또는 git fetch upstream

# 4. Rebase 실행
git rebase origin/main  # 또는 upstream/main

# 5-a. 충돌이 없는 경우: 완료
# 5-b. 충돌이 있는 경우:
#   - 충돌 파일 수정
#   - git add <수정한-파일>
#   - git rebase --continue
#   - 충돌 해결이 반복됨 (모든 커밋에 대해)

# 6. Rebase 성공 후 원격에 반영 (force push 필요)
git push --force-with-lease origin my-work-branch
```

**Rebase 중단/취소**:
```bash
# Rebase 진행 중 문제가 있으면 중단
git rebase --abort

# 백업 브랜치로 복구
git reset --hard backup/my-work-branch-before-rebase
```

**검증**:
```bash
# Rebase 결과 확인
git log --oneline --graph -n 10

# 백업과 비교
git log --oneline backup/my-work-branch-before-rebase..HEAD
```

---

### 시나리오 C: 로컬 Main을 원격과 완전히 동일하게 맞추기 (Hard Reset)

**목적**: 로컬 main 브랜치를 원격(upstream/origin)과 정확히 동일한 상태로 만들기

**전제조건**:
- 로컬 main의 변경사항을 모두 버려도 됨
- 원격의 main을 그대로 복제하고 싶음

**⚠️ 경고**:
- `reset --hard`는 로컬의 모든 변경사항을 영구적으로 삭제합니다
- 협업 중인 main 브랜치에 force push하면 다른 사람의 작업에 영향을 줄 수 있습니다
- 브랜치 보호 규칙이 설정되어 있으면 force push가 거부될 수 있습니다

**단계**:

```bash
# 1. main 브랜치로 이동
git checkout main

# 2. 백업 브랜치 생성 (필수!)
git branch backup/main-before-reset

# 3. 현재 main의 커밋 해시 기록
git rev-parse HEAD
# 출력: abc1234... (나중에 복구할 때 사용)

# 4. 최신 원격 정보 가져오기
git fetch origin  # 또는 git fetch upstream

# 5. 원격 main과 동일하게 Hard Reset
git reset --hard origin/main  # 또는 upstream/main

# 6. (선택) 원격에 강제 푸시 (주의!)
# 협업 브랜치가 아니고, 브랜치 보호 규칙이 없을 때만
git push --force-with-lease origin main
```

**검증**:
```bash
# 로컬과 원격이 동일한지 확인
git rev-parse main
git rev-parse origin/main
# 두 해시가 동일해야 함

# 차이 확인
git diff main origin/main
# 아무것도 출력되지 않아야 함
```

**복구**:
```bash
# 백업 브랜치로 복구
git reset --hard backup/main-before-reset

# 또는 reflog를 사용한 복구
git reflog
git reset --hard HEAD@{n}  # reflog에서 찾은 커밋으로 복구
```

---

### 시나리오 D: 작업 중인 변경사항을 Stash로 보관 후 Main 기준 작업

**목적**: 현재 작업 중인 변경사항을 임시로 저장하고, main 기준으로 새 작업 시작

**전제조건**:
- 워킹 디렉터리에 커밋하지 않은 변경사항이 있음
- 잠깐 다른 작업을 해야 하는 경우

**단계**:

```bash
# 1. 현재 변경사항 확인
git status

# 2. 변경사항을 stash에 저장
git stash push -m "WIP: 현재 작업 중인 내용 설명"

# 3. 최신 원격 정보 가져오기
git fetch origin  # 또는 git fetch upstream

# 4. Main 기준으로 새 브랜치 생성
git checkout -b feature/new-task origin/main

# 5. 새 작업 수행...

# 6. (나중에) 원래 브랜치로 돌아가서 stash 복원
git checkout original-branch
git stash list  # stash 목록 확인
git stash pop   # 또는 git stash apply stash@{0}
```

**Stash 관리**:
```bash
# Stash 목록 보기
git stash list

# 특정 stash 적용 (삭제하지 않음)
git stash apply stash@{0}

# 특정 stash 적용 및 삭제
git stash pop stash@{0}

# Stash 내용 확인
git stash show -p stash@{0}

# Stash 삭제
git stash drop stash@{0}

# 모든 stash 삭제
git stash clear
```

---

## 안전 수칙 및 복구 방법

### 백업 전략

**항상 백업 브랜치를 만드세요**:
```bash
# 작업 전 현재 브랜치 백업
git branch backup/my-branch-$(date +%Y%m%d-%H%M%S)

# 또는 간단하게
git branch backup/my-branch-before-operation
```

### Force Push 안전 수칙

**절대 사용하지 말 것**:
```bash
git push --force origin main  # 위험!
```

**대신 이것을 사용**:
```bash
git push --force-with-lease origin main
```

`--force-with-lease`의 장점:
- 원격 브랜치가 예상과 다른 상태면 푸시 거부
- 다른 사람이 먼저 푸시한 경우를 감지

### Reflog를 이용한 복구

Git은 모든 HEAD 변경 이력을 reflog에 저장합니다:

```bash
# Reflog 확인
git reflog

# 출력 예시:
# 4a98780 HEAD@{0}: commit: Initial plan
# 0ce25e1 HEAD@{1}: checkout: moving from main to feature
# abc1234 HEAD@{2}: reset: moving to origin/main

# 특정 시점으로 복구
git reset --hard HEAD@{2}

# 또는 특정 커밋 해시로 복구
git reset --hard abc1234
```

### 충돌 해결

**Merge 충돌**:
```bash
# 충돌 발생 시
git status  # 충돌 파일 확인

# 충돌 파일 수정 후
git add <파일>
git merge --continue

# 중단하려면
git merge --abort
```

**Rebase 충돌**:
```bash
# 충돌 발생 시
git status  # 충돌 파일 확인

# 충돌 파일 수정 후
git add <파일>
git rebase --continue

# 중단하려면
git rebase --abort
```

### 실수 복구 시나리오

**실수 1: 잘못된 파일을 커밋함**
```bash
# 마지막 커밋 취소 (변경사항은 유지)
git reset --soft HEAD~1

# 파일 수정 후 다시 커밋
git add .
git commit -m "올바른 커밋 메시지"
```

**실수 2: 잘못된 브랜치에서 커밋함**
```bash
# 1. 올바른 브랜치로 이동
git checkout correct-branch

# 2. 잘못된 브랜치의 커밋을 가져오기
git cherry-pick <커밋-해시>

# 3. 잘못된 브랜치에서 커밋 제거
git checkout wrong-branch
git reset --hard HEAD~1
```

**실수 3: 원격에 잘못 푸시함**
```bash
# 로컬에서 수정
git reset --hard <올바른-커밋>

# 강제 푸시 (협업 중이 아닐 때만!)
git push --force-with-lease origin branch-name
```

### 브랜치 보호 및 협업

**브랜치 보호 정책이 있는 경우**:
- Force push가 막혀 있을 수 있음
- Pull Request를 통해서만 변경 가능
- 관리자에게 문의하거나 PR을 생성하세요

**협업 중인 브랜치**:
- Rebase나 reset 전에 팀원에게 알리기
- 가능하면 merge를 사용하고, rebase는 개인 브랜치에서만
- Force push 전에 팀원이 해당 브랜치를 사용 중인지 확인

---

## 자주 사용하는 명령어 요약

### 상태 확인
```bash
git status                          # 워킹 디렉터리 상태
git branch -vv                      # 브랜치 목록 및 추적 정보
git log --oneline --graph -n 10     # 커밋 히스토리
git remote -v                       # 원격 저장소
git diff                            # 변경사항 확인
```

### 브랜치 작업
```bash
git checkout -b <브랜치명>          # 새 브랜치 생성 및 이동
git checkout <브랜치명>             # 브랜치 이동
git branch -d <브랜치명>            # 브랜치 삭제 (병합됨)
git branch -D <브랜치명>            # 브랜치 강제 삭제
```

### 원격 동기화
```bash
git fetch --all --prune            # 모든 원격 정보 가져오기
git pull origin main               # 원격 main 가져와서 병합
git push origin <브랜치명>         # 원격에 푸시
git push -u origin <브랜치명>      # 추적 설정하며 푸시
```

### 변경사항 관리
```bash
git add .                          # 모든 변경사항 스테이징
git add <파일>                     # 특정 파일 스테이징
git commit -m "메시지"             # 커밋
git commit --amend                 # 마지막 커밋 수정
git reset --soft HEAD~1            # 마지막 커밋 취소 (변경사항 유지)
git reset --hard HEAD~1            # 마지막 커밋 취소 (변경사항 삭제)
```

---

## 추가 자료

### Git 설정 권장사항

```bash
# 기본 에디터 설정
git config --global core.editor "vim"

# 기본 브랜치 이름 설정
git config --global init.defaultBranch main

# Pull 전략 설정 (rebase 선호)
git config --global pull.rebase true

# 자동 줄바꿈 설정 (Windows)
git config --global core.autocrlf true

# 자동 줄바꿈 설정 (Mac/Linux)
git config --global core.autocrlf input
```

### 유용한 Git Alias

```bash
# 단축 명령어 설정
git config --global alias.st status
git config --global alias.co checkout
git config --global alias.br branch
git config --global alias.ci commit
git config --global alias.lg "log --oneline --graph --decorate --all"
git config --global alias.unstage "reset HEAD --"
git config --global alias.last "log -1 HEAD"
```

---

## 문제 해결

### 일반적인 오류 해결

**"Your branch is ahead of 'origin/main' by N commits"**
```bash
# 로컬 커밋을 원격에 푸시
git push origin main
```

**"Your branch is behind 'origin/main' by N commits"**
```bash
# 원격 변경사항을 로컬로 가져오기
git pull origin main
```

**"fatal: refusing to merge unrelated histories"**
```bash
# 관련 없는 히스토리 강제 병합 (주의!)
git pull origin main --allow-unrelated-histories
```

**"error: failed to push some refs"**
```bash
# 원격 변경사항 먼저 가져오기
git pull origin main
# 충돌 해결 후 다시 푸시
git push origin main
```

---

이 가이드를 참고하여 안전하게 main 브랜치 기반으로 작업하세요!
