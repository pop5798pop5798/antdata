function NumberUpperFormat(input) {
    // num - 位数
    // 简单理解后面要有三个0，则是千，4个零，则是万。当然不一定是零，位数到了就行，反正都会省略掉（未做四舍五入）
    // 可以随意的增删改units内容实现单位的配置
    const units = [
        { num: 4, unit: '萬' },
        { num: 8, unit: '億' },
        { num: 12,unit: '兆'}
    ];
    // 精确到整数，直接用js自带方法input.toFixed(0)也可以
    const num = numeral(input).format('0');
    if (num.length <= 4) {
        // 项目相关需求，千以下不加单位
        return num;
    }
    // 保证前面至少留一位
    const len = num.length - 1;
    let isFind = false;
    for (let i = 0, length = units.length; i < length; i++) {
        let item = units[i];
        if (len >= item.num && len < units[i + 1].num) {
            isFind = true;
        } else if (i === (length - 2)) {
            isFind = true;
            item = units[++i];
        }
        if (isFind) {
            // 确认区间后，返回前几位加上单位
            return `${num.slice(0, num.length - item.num)}.${num.slice(num.length - item.num, num.length - item.num+1)}${item.unit}`;
        }
    }
}