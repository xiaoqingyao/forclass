测试版发布
====================

## 11.12

- 增加加入学习接口
 
  ```
    /api/PlatforUser/join
  ```

- 获取详情增加字段：
  ```
   CreationDate // 创建日期
   LeanerCount // 加入学习的人数
   SchoolName  // 创建者的学习名称
  ```

- 删除创建课程时参数: ``` goal```


## 11.13

- 增加课程标签属性接口（硬编码数据) 

    ```
    /api/Query/tag_prop
    ```

- 增加校提审和取消接口

    ```
    /api/Course/school_review // 校提审

    /api/Course/school_review_cancel //撤销校提审
    ```

- 课程详情Api 增加字段

    ```
        is_joined //是否已加入学习
        status //状态 可通过 /api/Query/course_status 查询状态说明
        singnature_id // 教育组Id
    ```

- 修改引用学程权限验证Bug


## 11.14

- 课程列表Api: ```/api/Query/course_list``` 
   
  - 增加加入学习人数字段 ```leaner_count```
  - 修改读取CourseId错误的Bug

- 增加取消加入的课程Api

  ``` /api/PlatforUser/leave ```

- 取消Session验证，部分Api可以匿名访问

## 11.14-2

- 课程列表更新
    - 增加搜索条件：```status // =1:草稿 =2: 校审核  =3: 全部 = 1|2```
    - 如果条件 ```is_learning = false``` 后台判断角色
    
      - 区域管理员或资源审核员 显示自己创建的或该区域下的所有课程
      - 校管理员或资源审核员 显示自己创建的或该学校下所有的课程
      - 普通角色 显示自己创始的 


## 11.19

- 校审核（通过、拒绝）、上下架、区域提审、取消提审API、
- 区域审核（通过、拒绝）、上下架Api
- 更新协作者Api（查询、添加、替换）
- 标签属性维护Api


## 12.03

- 腾迅接口添加创建时间字段，修改年级数据类型格式

- 修正审核时标签数据Bug

- 修正区域下架后无法拒绝的Bug

- 修正用户有未分组时系统异常，导致无法查看课程的Bug

- 修正课程上架、区域拒绝时无区域提审按钮的Bug

- 修正只有协作课程时，我的课程列表不显示筛选条件的Bug
