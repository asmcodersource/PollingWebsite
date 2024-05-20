import QuizQuestion from '../../Questions/QuizQuestion'
import Button from 'react-bootstrap/Button'
import './QuizQuestionAdminWrapper.css'

const QuizQuestionAdminWrapper = (props : any) => {
    return (
        <div className="quiz-question-admin-wrapper">
            <div className="admin-question-wrapper">
                <QuizQuestion question={props.question} />
            </div>
            <div className="buttons-wrapper">
                <Button onClick={() => props.up()}>Up</Button>
                <Button onClick={() => props.down()}>Down</Button>
                <Button onClick={() => props.edit()}>Edit</Button>
                <Button onClick={() => props.remove()}>Remove</Button>
            </div>
        </div>
    )
}

export default QuizQuestionAdminWrapper;