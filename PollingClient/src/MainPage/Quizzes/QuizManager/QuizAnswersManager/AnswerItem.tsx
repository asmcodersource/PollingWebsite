import './AnswerItem.css'


export default function AnswerItem(props: any) {

    function shortenTime(isoString: string): string {
        if (!/^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{1,7}$/.test(isoString)) {
            throw new Error("Invalid ISO 8601 string format");
        }

        const [date, time] = isoString.split('T');
        const [hours, minutes] = time.split(':');
        return `${date} ${hours}:${minutes}`;
    }

    return (
        <div className="answer-item-wrapper" onClick={() => props.onClick(props.answer)}>
            <span>Nickname: <label>{props.answer.userNickname}</label></span>
            <span>Answer time: <span className="time">{shortenTime(props.answer.answerTime)}</span></span>
        </div>
    )
}