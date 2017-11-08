import React from 'react';
import PropTypes from 'prop-types';

export default class InputPane extends React.Component {
    render() {
        return (
			<div>
				<div className="input-box">
					<textarea defaultValue={"Enter some text (news article...)!"} rows="10" />
				</div>
			    <button onClick={() => this.props.addFact(": ^ )")}>
			        AddFact
			    </button>
			</div>
        );
    }
}

InputPane.propTypes = {
    addFact: PropTypes.func.isRequired
}